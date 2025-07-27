namespace Assesment_Fortitude.Calculation
{
    public class TotalAmountCalculation
    {
        public static long CalculateDiscount(long totalAmount)
        {
            long baseDiscount = 0;
            long conditionalDiscount = 0;
            if (totalAmount >= 20000 && totalAmount <= 50000)
                baseDiscount = 5;
            else if (totalAmount >= 50100 && totalAmount <= 80000)
                baseDiscount = 7;
            else if (totalAmount >= 80100 && totalAmount <= 120000)
                baseDiscount = 10;
            else if (totalAmount > 120000)
                baseDiscount = 15; 

            if (totalAmount > 50000 && IsPrime((long)totalAmount))
                conditionalDiscount += 8;

            if (totalAmount > 90000 && totalAmount % 10 == 5)
                conditionalDiscount += 10;
             
            long totalDiscount = baseDiscount + conditionalDiscount;
            if (totalDiscount > 20)
                totalDiscount = 20;

            return totalDiscount;
        }

        /// <summary> 
        /// divisible by 2 or 3 (not prime), must be greater than 1
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static bool IsPrime(long number)
        {
            if (number <= 1) return false;
            if (number % 2 == 0 && number != 2) return false;
            if (number % 3 == 0) return false;

            return true;
        }
    }
}
