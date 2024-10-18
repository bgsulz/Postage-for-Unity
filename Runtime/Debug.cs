using System;
using System.Collections.Generic;
using UnityEngine;

namespace Extra.Postman
{
    public class Report
    {
        public CallerInfo CallerInfo { get; }
        public Address Address { get; }
        public object Parcel { get; }

        public Report(CallerInfo callerInfo, Address address, object parcel)
        {
            CallerInfo = callerInfo;
            Address = address;
            Parcel = parcel;
        }

        public override string ToString() => $"{Parcel} -> {Address.Key}";
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
        private static int _maxStoredReports = 32;
        private static Queue<Report> _storedReports = new(_maxStoredReports);

        public static int MaxStoredReports
        {
            get => _maxStoredReports;
            set
            {
                if (value < 0) return;
                _maxStoredReports = value;
                _storedReports = new Queue<Report>(_maxStoredReports);
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