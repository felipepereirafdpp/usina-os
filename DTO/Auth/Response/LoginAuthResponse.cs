namespace UsinaOS.DTO.Auth.Response
{
    public class LoginAuthResponse
    {
         public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiraEm { get; set; }
        public bool DeveAlterarSenha { get; set; }
    }
}
