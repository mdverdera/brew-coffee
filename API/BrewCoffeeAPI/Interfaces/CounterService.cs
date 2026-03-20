namespace BrewCoffeeAPI.Interfaces
{
    public class CounterService : ICounterService
    {
        private int _counter = 0;

        public int Increment()
        {
            return Interlocked.Increment(ref _counter);
        }
    }
}
