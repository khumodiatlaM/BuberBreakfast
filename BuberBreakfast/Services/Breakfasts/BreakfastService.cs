using BuberBreakfast.Entities;
using BuberBreakfast.ServiceErrors;
using ErrorOr;

namespace BuberBreakfast.Services.Breakfasts
{
    public class BreakfastService : IBreakfastService
    {
        private static readonly Dictionary<Guid, Breakfast> _breakfast = new();
        public void CreateBreakfast(Breakfast breakfast)
        {
            _breakfast.Add(breakfast.Id, breakfast);
        }

        public void DeleteBreakfast(Guid id)
        {
            _breakfast.Remove(id);
        }

        public ErrorOr<Breakfast> GetBreakfast(Guid id)
        {
            if (!_breakfast.ContainsKey(id)) return Errors.Breakfast.NotFound;

            return _breakfast[id];
        }
        public void UpsertBreakfast(Breakfast breakfast)
        {
            _breakfast[breakfast.Id] = breakfast;
        }
    }
}
