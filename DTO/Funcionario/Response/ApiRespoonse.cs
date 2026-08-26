namespace UsinaOS.DTO.Funcionario.Response
{
    public class ApiRespoonse<T>
    {
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
