/****************
Mã sinh viên: 202418906
Họ tên: Lê Gia Huân
****************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab06
{
    /// <summary>Phòng học, kết tập danh sách thiết bị.</summary>
    public class ClassRoom
    {
        private readonly List<Device> _devices = new List<Device>();

        public string RoomId { get; }
        public string RoomName { get; }
        public int Capacity { get; }
        public IReadOnlyList<Device> Devices => _devices;

        public ClassRoom(string roomId, string roomName, int capacity)
        {
            if (string.IsNullOrWhiteSpace(roomId))
                throw new ArgumentException("Mã phòng không được rỗng.");
            if (string.IsNullOrWhiteSpace(roomName))
                throw new ArgumentException("Tên phòng không được rỗng.");
            if (capacity <= 0)
                throw new ArgumentException("Sức chứa phải lớn hơn 0.");

            RoomId = roomId;
            RoomName = roomName;
            Capacity = capacity;
        }

        public void AddDevice(Device device)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device), "Thiết bị không được null.");
            if (_devices.Any(d => d.DeviceId == device.DeviceId))
                throw new InvalidOperationException($"Thiết bị {device.DeviceId} đã có trong phòng {RoomId}.");
            _devices.Add(device);
        }

        public bool RemoveDevice(string deviceId)
        {
            var device = FindDevice(deviceId);
            return device != null && _devices.Remove(device);
        }

        public Device? FindDevice(string deviceId)
        {
            return _devices.FirstOrDefault(d => d.DeviceId == deviceId);
        }

        // Đa hình: gọi phương thức trừu tượng, không kiểm tra kiểu bằng if/switch
        public decimal CalculateAnnualMaintenanceCost()
        {
            return _devices.Sum(d => d.CalculateAnnualMaintenanceCost());
        }

        public List<Device> GetDevicesRequiringMaintenance()
        {
            return _devices
                .Where(d => d.Status == DeviceStatus.UnderMaintenance || d.GetAge() > 5)
                .ToList();
        }
    }
}
