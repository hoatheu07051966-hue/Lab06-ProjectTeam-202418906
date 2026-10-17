/****************
Mã sinh viên: 202418906
Họ tên: Lê Gia Huân
****************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab06
{
    /// <summary>Phiếu mượn: hợp thành các BorrowItem, tham chiếu người mượn.</summary>
    public class BorrowingSlip
    {
        private readonly List<BorrowItem> _items = new List<BorrowItem>();

        public string SlipId { get; }
        public IBorrower Borrower { get; }
        public string TeachingRoom { get; }
        public DateTime BorrowedDate { get; }
        public IReadOnlyList<BorrowItem> Items => _items;

        public BorrowingSlip(string slipId, IBorrower borrower, string teachingRoom, DateTime borrowedDate)
        {
            if (string.IsNullOrWhiteSpace(slipId))
                throw new ArgumentException("Mã phiếu không được rỗng.");
            if (borrower == null)
                throw new ArgumentNullException(nameof(borrower), "Người mượn không được null.");
            if (string.IsNullOrWhiteSpace(teachingRoom))
                throw new ArgumentException("Phòng dạy không được rỗng.");

            SlipId = slipId;
            Borrower = borrower;
            TeachingRoom = teachingRoom;
            BorrowedDate = borrowedDate;
        }

        /// <summary>Kiểm tra toàn bộ điều kiện TRƯỚC khi thay đổi bất kỳ trạng thái nào.</summary>
        private void ValidateBorrowing(TeachingDevice device, int borrowDays)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device), "Thiết bị không được null.");
            if (borrowDays <= 0)
                throw new InvalidBorrowPeriodException(device.DeviceId, borrowDays, device.MaxBorrowPeriods);
            if (borrowDays > device.MaxBorrowPeriods)
                throw new InvalidBorrowPeriodException(device.DeviceId, borrowDays, device.MaxBorrowPeriods);
            if (_items.Any(i => i.Device.DeviceId == device.DeviceId))
                throw new DuplicateDeviceException(device.DeviceId);
            if (_items.Count >= Borrower.GetMaximumDevices())
                throw new BorrowLimitExceededException(Borrower.BorrowerId, Borrower.GetMaximumDevices());
            if (device.Status != DeviceStatus.Available)
                throw new DeviceUnavailableException(device.DeviceId, device.Status);
        }

        public void AddDevice(TeachingDevice device, int borrowDays)
        {
            ValidateBorrowing(device, borrowDays);          // 1. kiểm tra hợp lệ

            var item = new BorrowItem(device, BorrowedDate, BorrowedDate.AddDays(borrowDays)); // 2. tạo item
            device.MarkAsBorrowed();                        // 3. đổi trạng thái thiết bị
            _items.Add(item);                               // 4. thêm vào phiếu
        }

        public decimal ReturnDevice(string deviceId, DateTime returnedDate)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
                throw new ArgumentException("Mã thiết bị không được rỗng.");

            var item = _items.FirstOrDefault(i => i.Device.DeviceId == deviceId);
            if (item == null)
                throw new DeviceNotInSlipException(deviceId);

            // Return() kiểm tra "đã trả" và "ngày trả hợp lệ" trước khi ghi nhận
            item.Return(returnedDate);
            item.Device.MarkAsAvailable();   // chỉ đổi trạng thái khi việc trả đã thành công

            return item.CalculateLateFee();
        }

        public decimal CalculateTotalLateFee()
        {
            return _items.Where(i => i.IsReturned).Sum(i => i.CalculateLateFee());
        }
    }
}
