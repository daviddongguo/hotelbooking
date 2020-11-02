using System;

namespace david.hotelbooking.domain.Entities
{
    public class ServiceResponse<T>
    {
        public string Message { get; set; } = null;
        public T Data { get; set; }

        public object FirstOrDefault()
        {
            throw new NotImplementedException();
        }
    }
}
