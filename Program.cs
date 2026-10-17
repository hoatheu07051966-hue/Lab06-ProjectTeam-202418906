/****************
Mã sinh viên: 202418906
Họ tên: Lê Gia Huân
****************/

using System;

namespace Lab06
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            RunSuccessFlow();
            RunExceptionFlow();
        }

        // 11.1 Luồng thành công
        static void RunSuccessFlow()
        {
            Console.WriteLine("=== 11.1. LUỒNG THÀNH CÔNG ===");

            var lecturer = new Lecturer("GV01", "Nguyễn Văn A");
            var projector = new ProjectorRemoteControl("PRJ01", "Điều khiển máy chiếu Epson", 2023, 500000m);
            var ac = new AirConditionerRemoteControl("AC01", "Điều khiển điều hòa Daikin", 2022, 300000m);
            var mic = new Micro("MIC01", "Micro không dây Shure", 2024, 1200000m, 3);
            var mic2 = new Micro("MIC02", "Micro không dây Sony", 2024, 1200000m, 7);

            var room = new ClassRoom("P101", "Phòng 101", 60);
            room.AddDevice(projector);
            room.AddDevice(ac);
            room.AddDevice(mic);
            Console.WriteLine("1. Tạo giảng viên và các thiết bị giảng dạy.");
            Console.WriteLine("2. Thêm thiết bị vào phòng học.");
            Console.WriteLine($"3. Chi phí bảo trì dự kiến/năm của phòng: {room.CalculateAnnualMaintenanceCost():N0} VND");

            var slip = new BorrowingSlip("PM01", lecturer, "P101", new DateTime(2026, 10, 10));

            slip.AddDevice(projector, 5);
            slip.AddDevice(mic, 3);
            slip.AddDevice(mic2, 6);
            Console.WriteLine("4. Thêm điều khiển máy chiếu và micro vào phiếu.");

            slip.ReturnDevice("PRJ01", new DateTime(2026, 10, 15));   // đúng hạn
            Console.WriteLine("5. Trả điều khiển máy chiếu đúng hạn.");

            slip.ReturnDevice("MIC01", new DateTime(2026, 10, 15));   // hạn 13/10 -> muộn 2 ngày
            Console.WriteLine("6. Trả micro muộn 2 ngày.");

            Console.WriteLine($"Phí phạt cho phiếu: {slip.CalculateTotalLateFee():0} VND");

            Console.WriteLine("7. Hiển thị các thiết bị trả muộn, chưa trả trong phiếu:");
            foreach (var item in slip.Items)
            {
                if (!item.IsReturned)
                    Console.WriteLine($"   - [Chưa trả] Thiết bị {item.Device.DeviceId}. Hạn trả: {item.DueDate:dd/MM/yyyy}");
                else if (item.GetLateDays() > 0)
                    Console.WriteLine($"   - [Trả muộn] Thiết bị {item.Device.DeviceId}. Trễ {item.GetLateDays()} ngày.");
            }
            Console.WriteLine();
        }

        // 11.2 Luồng phát sinh ngoại lệ
        static void RunExceptionFlow()
        {
            Console.WriteLine("=== 11.2. LUỒNG PHÁT SINH NGOẠI LỆ ===");

            var lecturer = new Lecturer("GV01", "Nguyễn Văn A");
            var assistant = new TeachingAssistant("TA01", "Trần Thị B");
            var borrowedDate = new DateTime(2026, 10, 10);

            // 1. Giảng viên mượn thiết bị thứ tư
            Try("1", () =>
            {
                var slip = new BorrowingSlip("PM02", lecturer, "P101", borrowedDate);
                slip.AddDevice(new Micro("M01", "Micro 1", 2024, 1000000m, 3), 1);
                slip.AddDevice(new Micro("M02", "Micro 2", 2024, 1000000m, 3), 1);
                slip.AddDevice(new Micro("M03", "Micro 3", 2024, 1000000m, 3), 1);
                slip.AddDevice(new Micro("M0X", "Micro X", 2024, 1000000m, 3), 1);
            });

            // 2. Trợ giảng mượn thiết bị thứ ba
            Try("2", () =>
            {
                var slip = new BorrowingSlip("PM03", assistant, "P102", borrowedDate);
                slip.AddDevice(new Micro("MIC02", "Micro 2", 2024, 1000000m, 3), 1);
                slip.AddDevice(new Micro("MIC03", "Micro 3", 2024, 1000000m, 3), 1);
                slip.AddDevice(new Micro("MIC04", "Micro 4", 2024, 1000000m, 3), 1);
            });

            // 3. Thêm cùng một micro hai lần
            Try("3", () =>
            {
                var mic = new Micro("M04", "Micro 4", 2024, 1000000m, 3);
                var slip = new BorrowingSlip("PM04", lecturer, "P101", borrowedDate);
                slip.AddDevice(mic, 1);
                slip.AddDevice(mic, 1);
            });

            // 4. Mượn micro 3 ngày dù giới hạn là 1 ngày
            Try("4", () =>
            {
                var mic = new Micro("M05", "Micro 5", 2024, 1000000m, 1);
                new BorrowingSlip("PM05", lecturer, "P101", borrowedDate).AddDevice(mic, 3);
            });

            // 5. Trả điều khiển máy chiếu không thuộc phiếu
            Try("5", () =>
            {
                var slip = new BorrowingSlip("PM06", lecturer, "P101", borrowedDate);
                slip.ReturnDevice("THIET_BI_MA", borrowedDate.AddDays(1));
            });

            // 6. Trả cùng một thiết bị hai lần
            Try("6", () =>
            {
                var slip = new BorrowingSlip("PM07", lecturer, "P101", borrowedDate);
                slip.AddDevice(new Micro("MC01", "Micro C1", 2024, 1000000m, 3), 2);
                slip.ReturnDevice("MC01", borrowedDate.AddDays(1));
                slip.ReturnDevice("MC01", borrowedDate.AddDays(1));
            });

            // 7. Trả thiết bị với ngày trả trước ngày mượn
            Try("7", () =>
            {
                var slip = new BorrowingSlip("PM08", lecturer, "P101", borrowedDate);
                slip.AddDevice(new Micro("MIC08", "Micro 8", 2024, 1000000m, 3), 2);
                slip.ReturnDevice("MIC08", borrowedDate.AddDays(-1));
            });

            // Kiểm tra tính nhất quán sau khi thất bại
            var mic9 = new Micro("MIC09", "Micro 9", 2024, 1000000m, 1);
            var slip9 = new BorrowingSlip("PM09", lecturer, "P101", borrowedDate);
            try { slip9.AddDevice(mic9, 5); }
            catch (BorrowingException) { }
            if (mic9.Status != DeviceStatus.Available || slip9.Items.Count != 0)
                Console.WriteLine("Lỗi: trạng thái không nhất quán sau khi thất bại.");
        }

        static void Try(string no, Action action)
        {
            try
            {
                action();
                Console.WriteLine($"{no}. (Không có ngoại lệ)");
            }
            catch (BorrowingException ex)
            {
                Console.WriteLine($"{no}. {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{no}. [Lỗi khác - {ex.GetType().Name}] {ex.Message}");
            }
            finally
            {
                // finally: chạy dù có hay không có ngoại lệ
            }
        }
    }
}
