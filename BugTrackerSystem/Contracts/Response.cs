namespace BugTrackerAPI.Contracts
{
    public class ApiResponse
    {
        public Exception Exception { get; set; }
        public object Data { get; set; }
        public int StatusCode { get; set; }
    }

    public class Response : IActionResult
    {
        //private readonly ApiResponse _apiResponse;

        //public Response(ApiResponse apiResponse)
        //{
        //    _apiResponse = apiResponse;
        //}

        private object _data;
        private string _statusMessage;
        private int _statusCode;
        private Exception _exception;

        public Response(object data, int statusCode = 200, string statusMessage = "OK", Exception? exception = null)
		{
			_data = data;
			_statusMessage = statusMessage;
			_statusCode = statusCode;
			_exception = exception;
		}

		public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(_exception ?? _data)
			{
				StatusCode = _exception != null
					? StatusCodes.Status500InternalServerError
					: _statusCode
			};

			await objectResult.ExecuteResultAsync(context);
		}
    }
}
