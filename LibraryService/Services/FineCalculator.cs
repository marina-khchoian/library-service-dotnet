using LibraryService.Entities;

namespace LibraryService.Services
{
    public static class FineCalculator
    {
        public const decimal FinePerDay = 0.50m;

        // რამდენი სრული დღით დააგვიანა: დაბრუნებულზე — დაბრუნების დღემდე, აქტიურზე — დღემდე
        public static int LateDays(BorrowRecord record)
        {
            var end = record.ReturnedAt ?? DateTime.UtcNow;
            return end > record.DueDate ? (int)(end - record.DueDate).TotalDays : 0;
        }

        public static decimal Fine(BorrowRecord record) => LateDays(record) * FinePerDay;

        // გადაუხდელი ჯარიმა: თანხა > 0 და FinePaidAt ცარიელია
        public static bool HasUnpaidFine(BorrowRecord record) =>
            record.FinePaidAt == null && Fine(record) > 0;
    }
}