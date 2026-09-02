namespace UsinaOS.Exceptions.Cliente
{

    public class ValidaRazaoSocialException : Exception
    {
        public ValidaRazaoSocialException() : base("A  informado é invalido") { }
        public ValidaRazaoSocialException(string message) : base(message) { }

        public ValidaRazaoSocialException(string message, Exception innerException) : base(message, innerException) { }


    }
}
