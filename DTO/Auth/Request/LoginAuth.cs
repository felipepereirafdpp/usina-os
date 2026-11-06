namespace UsinaOS.DTO.Auth.Request
{
    public class LoginAuth
    {
        public string CPF { get; set; }
        public string SenhaHash { get; set; }
    }
}
