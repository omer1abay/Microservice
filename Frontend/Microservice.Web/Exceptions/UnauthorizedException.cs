using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Microservice.Web.Exceptions
{
    public class UnauthorizedException : Exception
    {
        //custom exception yazmanın en önemli faydası eğer program kendi hata fırlatırsa onu tespit etmemiz zordur ama custom exception fırlatırsak bu exception'ın nereden geldiğinin takibi daha rahat olur
        //default constructor'ları unutmamalıyız
        public UnauthorizedException()
        {
        }

        public UnauthorizedException(string message) : base(message)
        {
        }

        public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
