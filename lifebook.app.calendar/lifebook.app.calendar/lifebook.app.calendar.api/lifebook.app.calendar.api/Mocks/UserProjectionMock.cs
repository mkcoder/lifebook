using System;
using System.Collections.Generic;

namespace lifebook.app.calendar.api.Mocks
{
    public static class UserProjectionMock
    {
        private static Guid[] _guids = new Guid[]{
            Guid.Parse("B914B6C9-5A92-4C0B-AA9E-DF665DE14861"),
            Guid.Parse("b72a3de6-04fc-42f2-9fd7-ce9119d1d9a0"), // returns from the mock
            Guid.Parse("2E9106D0-BDFC-423F-97EE-9A1D57609FF3"),
            Guid.Parse("820DFD52-0A09-4649-A23D-716FE871D071"),
            Guid.Parse("9A122086-208C-4C3D-9932-38DDBA2D58A0"),
            Guid.Parse("71E9432D-540B-420E-9FFA-8C2E5EDC22B5"),
            Guid.Parse("FDBDD9FB-A8F1-4FB4-B48A-201529CDF723"),
            Guid.Parse("2E9654C1-2F36-4690-8060-543B18251A1A"),
            Guid.Parse("135354B8-6ABB-40BD-B056-BAAF03FEC0DC"),
            Guid.Parse("3E6B49EB-111A-426C-8EBA-3C1602DC3CCF")
        };

        private static Dictionary<Guid, Calendar>  _projection = new Dictionary<Guid, Calendar>()
        {
            { _guids[0], CalendarProjectionMock.Projection(DateTime.Now, DateTime.Now.AddDays(30)) },
            { _guids[1], CalendarProjectionMock.Projection(DateTime.Now.AddMonths(1), DateTime.Now.AddMonths(2).AddDays(30)) },
            { _guids[2], CalendarProjectionMock.Projection(DateTime.Now.AddMonths(2), DateTime.Now.AddMonths(2).AddDays(30)) },
            { _guids[3], CalendarProjectionMock.Projection(DateTime.Now.AddMonths(3), DateTime.Now.AddMonths(3).AddDays(30)) },
            { _guids[4], CalendarProjectionMock.Projection(DateTime.Now.AddMonths(5), DateTime.Now.AddMonths(5).AddDays(30)) },
            { _guids[5], CalendarProjectionMock.Projection(DateTime.Now.AddMonths(6), DateTime.Now.AddMonths(6).AddDays(30)) },
            { _guids[6], CalendarProjectionMock.Projection(DateTime.Now.AddMonths(7), DateTime.Now.AddMonths(7).AddDays(30)) },
            { _guids[7], CalendarProjectionMock.Projection(DateTime.Now.AddMonths(8), DateTime.Now.AddMonths(8).AddDays(30)) },
            { _guids[8], CalendarProjectionMock.Projection(DateTime.Now.AddMonths(9), DateTime.Now.AddMonths(9).AddDays(30)) },
        };

        public static Calendar GetCalendarByUserID(Guid userId)
        {
            return _projection[userId];
        }
    }
}
