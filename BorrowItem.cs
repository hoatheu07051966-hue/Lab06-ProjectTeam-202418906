/****************
Mã sinh viên: 202418906
Họ tên: Lê Gia Huân
****************/

using System;

namespace Lab06
{
    /// <summary>Chi tiết một thiết bị được mượn trong phiếu.</summary>
    public class BorrowItem
    {
        public const decimal LateFeePerDay = 10000m;

        public TeachingDevice Device { get; }
        public DateTime BorrowedDate { get; }
        public DateTime DueDate { get; }
        public int FromPeriod { get; set; }
        public int ToPeriod { get; set; }
        public DateTime? ReturnedDate { get; private set; }
        public bool IsReturned => ReturnedDate.HasValue;

        public BorrowItem(TeachingDevice device, DateTime borrowedDate, DateTime dueDate,
                          int fromPeriod = 1, int toPeriod = 1)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device), "Thiết bị không được null.");
            if (dueDate < borrowedDate)
                throw new ArgumentException("Hạn trả phải sau hoặc bằng ngày mượn.");
            if (toPeriod < fromPeriod)
                throw new ArgumentException("Tiết đến phải sau hoặc bằng tiết mượn.");

            Device = device;
            BorrowedDate = borrowedDate;
            DueDate = dueDate;
            FromPeriod = fromPeriod;
            ToPeriod = toPeriod;
            ReturnedDate = null;
        }

        public void Return(DateTime returnedDate)
        {
            if (IsReturned)
                throw new DeviceAlreadyReturnedException(Device.DeviceId);
            if (returnedDate.Date < BorrowedDate.Date)
                throw new InvalidReturnDateException(BorrowedDate, returnedDate);

            ReturnedDate = returnedDate;
        }

        /// <summary>Số ngày trả muộn (0 nếu đúng hạn hoặc chưa trả).</summary>
        public int GetLateDays()
        {
            if (!IsReturned) return 0;
            int days = (ReturnedDate!.Value.Date - DueDate.Date).Days;
            return days > 0 ? days : 0;
        }

        public decimal CalculateLateFee() => GetLateDays() * LateFeePerDay;
    }
}
