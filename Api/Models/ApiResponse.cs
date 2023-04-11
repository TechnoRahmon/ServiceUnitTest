namespace Api.Models
{
    public class ApiResponse<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public ApiResponse()
        {
            Errors = new List<string>();
        }
        /// <summary>
        /// Create Response Object
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResponse<T> Create(T data)
        {
            return new ApiResponse<T>()
            {
                Data = data
            };
        }
        /// <summary>
        /// Create Failure object with single error message
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ApiResponse<T> CreateFailure(string msg)
        {
            return new ApiResponse<T>()
            {
                Errors = new List<string>() { msg }
            };
        }
        /// <summary>
        /// Create Failure object with list of error messages
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ApiResponse<T> CreateFailure(List<string> msg)
        {
            return new ApiResponse<T>()
            {
                Errors = msg
            };
        }
        /// <summary>
        /// Data response.
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// Check if an exception occurred.
        /// </summary>
        public bool Status => !this.Errors.Any();
        /// <summary>
        /// Return list of errors if exists.
        /// </summary>
        public List<string> Errors { get; set; }
        /// <summary>
        /// Request Id
        /// </summary>
        public string RequestId = Guid.NewGuid().ToString("D");
        /// <summary>
        /// Response Id
        /// </summary>
        public DateTime TimeStamp = DateTime.Now;
    }
}
