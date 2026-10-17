/****************
Mã sinh viên: 202418906
Họ tên: Lê Gia Huân
****************/

using System;

namespace Lab06
{
    public interface IBorrower
    {
        string BorrowerId { get; }
        string FullName { get; }
        int GetMaximumDevices();
    }

    /// <summary>Giảng viên: mượn tối đa 3 thiết bị / phiếu.</summary>
    public class Lecturer : IBorrower
    {
        public string BorrowerId { get; }
        public string FullName { get; }

        public Lecturer(string borrowerId, string fullName)
        {
            if (string.IsNullOrWhiteSpace(borrowerId))
                throw new ArgumentException("Mã người mượn không được rỗng.");
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Họ tên không được rỗng.");
            BorrowerId = borrowerId;
            FullName = fullName;
        }

        public int GetMaximumDevices() => 3;
    }

    /// <summary>Trợ giảng: mượn tối đa 2 thiết bị / phiếu.</summary>
    public class TeachingAssistant : IBorrower
    {
        public string BorrowerId { get; }
        public string FullName { get; }

        public TeachingAssistant(string borrowerId, string fullName)
        {
            if (string.IsNullOrWhiteSpace(borrowerId))
                throw new ArgumentException("Mã người mượn không được rỗng.");
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Họ tên không được rỗng.");
            BorrowerId = borrowerId;
            FullName = fullName;
        }

        public int GetMaximumDevices() => 2;
    }
}
