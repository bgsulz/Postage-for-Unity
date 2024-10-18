using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extra.Postman
{
    public class Report
    {
        public CallerInfo CallerInfo { get; }
        public Address Address { get; }
        public object Parcel { get; }

        public Type ParcelType { get; }
        public string ParcelTypeAsString { get; }
        public string ParcelAsString { get; }

        public Report(CallerInfo callerInfo, Address address, object parcel)
        {
            CallerInfo = callerInfo;
            Address = address;
            Parcel = parcel;

            ParcelType = Parcel.GetType();
            ParcelTypeAsString = CalculateStringType(ParcelType);
            ParcelAsString = CalculateStringRepresentation(Parcel);
        }

        private string CalculateStringType(Type type)
        {
            if (type == typeof(int)) return "int";
            if (type == typeof(float)) return "float";
            if (type == typeof(string)) return "string";
            if (type == typeof(Color)) return "Color";
            if (type == typeof(Vector2)) return "Vector2";
            if (type == typeof(Vector3)) return "Vector3";
            if (type == typeof(Vector2Int)) return "Vector2Int";
            if (type == typeof(Vector3Int)) return "Vector3Int";
            return type.ToString().Split(".").Last();
        }

        private string CalculateStringRepresentation(object parcel)
        {
            if (parcel == null) return "--null--";
            if (parcel is not string str) return parcel.ToString();
            if (str == string.Empty) return "--empty string--";
            if (string.IsNullOrWhiteSpace(str)) return $"--whitespace:{str}--";
            return str;
        }

        public override string ToString() => $"{ParcelAsString} -> {Address.Key}";
    }

    public class CallerInfo
    {
        public string CallerMemberName { get; }
        public string SourceFilePath { get; }
        public int SourceLineNumber { get; }

        private string _truncatedPath;
        public string TruncatedPath => _truncatedPath ??= GetTruncatedPath();

        private string GetTruncatedPath()
        {
            var index = Mathf.Max(SourceFilePath.IndexOf("Assets"), SourceFilePath.IndexOf("Packages"));
            if (index < 0) return SourceFilePath;
            return SourceFilePath[index..];
        }

        public CallerInfo(string callerMemberName, string sourceFilePath, int sourceLineNumber)
        {
            CallerMemberName = callerMemberName;
            SourceFilePath = sourceFilePath;
            SourceLineNumber = sourceLineNumber;
        }

        public override string ToString() => $"{CallerMemberName} at {TruncatedPath}:{SourceLineNumber}";
    }

    public static class Reports
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _maxStoredReports = 128;
            _storedReports = new(_maxStoredReports);
        }

        private static int _maxStoredReports = 128;
        private static Queue<Report> _storedReports = new(_maxStoredReports);

        public static int MaxStoredReports
        {
            get => _maxStoredReports;
            set
            {
                if (value < 0) return;
                _maxStoredReports = value;
                _storedReports = new(_maxStoredReports);
            }
        }

        public static Queue<Report> StoredReports => _storedReports;

        public static void Report(string callerMemberName, string sourceFilePath, int sourceLineNumber, Address address, object parcel)
        {
            var callerInfo = new CallerInfo(callerMemberName, sourceFilePath, sourceLineNumber);
            _storedReports.Enqueue(new(callerInfo, address, parcel));
            if (_storedReports.Count > _maxStoredReports) _storedReports.Dequeue();
        }

        public static void ClearReports() => _storedReports.Clear();
    }
}