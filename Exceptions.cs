/****************
Mã sinh viên: 202418906
Họ tên: Lê Gia Huân
****************/

using System;

namespace Lab06
{
    public class BorrowingException : Exception
    {
        public BorrowingException(string message) : base(message) { }

        public BorrowingException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class DeviceUnavailableException : BorrowingException
    {
        public string DeviceId { get; }
        public DeviceStatus CurrentStatus { get; }

        public DeviceUnavailableException(string deviceId, DeviceStatus currentStatus)
            : base($"Thiết bị {deviceId} không sẵn sàng (trạng thái: {currentStatus}).")
        {
            DeviceId = deviceId;
            CurrentStatus = currentStatus;
        }
    }

    public class BorrowLimitExceededException : BorrowingException
    {
        public string BorrowerId { get; }
        public int MaximumDevices { get; }

        public BorrowLimitExceededException(string borrowerId, int maximumDevices)
            : base($"Người mượn {borrowerId} đã vượt quá giới hạn {maximumDevices} thiết bị.")
        {
            BorrowerId = borrowerId;
            MaximumDevices = maximumDevices;
        }
    }

    public class InvalidBorrowPeriodException : BorrowingException
    {
        public string DeviceId { get; }
        public int RequestedDays { get; }
        public int MaxDays { get; }

        public InvalidBorrowPeriodException(string deviceId, int requestedDays, int maxDays)
            : base($"Thiết bị {deviceId} cho phép mượn tối đa {maxDays} ngày, yêu cầu: {requestedDays} ngày.")
        {
            DeviceId = deviceId;
            RequestedDays = requestedDays;
            MaxDays = maxDays;
        }
    }

    public class DuplicateDeviceException : BorrowingException
    {
        public string DeviceId { get; }

        public DuplicateDeviceException(string deviceId)
            : base($"Thiết bị {deviceId} đã có trong phiếu.")
        {
            DeviceId = deviceId;
        }
    }

    public class DeviceNotInSlipException : BorrowingException
    {
        public string DeviceId { get; }

        public DeviceNotInSlipException(string deviceId)
            : base($"Thiết bị {deviceId} không thuộc phiếu mượn.")
        {
            DeviceId = deviceId;
        }
    }

    public class DeviceAlreadyReturnedException : BorrowingException
    {
        public string DeviceId { get; }

        public DeviceAlreadyReturnedException(string deviceId)
            : base($"Thiết bị {deviceId} đã được trả.")
        {
            DeviceId = deviceId;
        }
    }

    // Ngoại lệ bổ sung: tách riêng lỗi "ngày trả không hợp lệ" để thông báo rõ nguyên nhân
    // (đề yêu cầu xử lý tình huống này nhưng chưa có lớp ngoại lệ tương ứng).
    public class InvalidReturnDateException : BorrowingException
    {
        public DateTime BorrowedDate { get; }
        public DateTime ReturnedDate { get; }

        public InvalidReturnDateException(DateTime borrowedDate, DateTime returnedDate)
            : base("Ngày trả không trước ngày mượn.")
        {
            BorrowedDate = borrowedDate;
            ReturnedDate = returnedDate;
        }
    }
}
