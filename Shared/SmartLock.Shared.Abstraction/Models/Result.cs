namespace SmartLock.Shared.Abstraction.Models {
    public record Result {
        public Result(ResultCode code = ResultCode.Ok, params string[] messages) {
            Code = code;
            Messages = messages;
        }

        public ResultCode Code { get; private set; }
        public IEnumerable<string> Messages { get; private set; } = null;
        public bool Success => Code == ResultCode.Ok;

        public static Result BadRequest(params string[] messages) =>
            new Result(ResultCode.BadRequest, messages);

        public static Result Successed(string message = null) =>
            new Result(messages: message);
    }

    public record Result<T> : Result {
        public Result(ResultCode code = ResultCode.Ok, params string[] messages)
            : base(code, messages) {
        }

        public Result(T data, ResultCode code = ResultCode.Ok, params string[] messages)
            : base(code, messages) {
            Data = data;
        }

        public T Data { get; set; }

        public new static Result<T> BadRequest(params string[] messages) =>
             new Result<T>(ResultCode.BadRequest, messages);

        public new static Result<T> Successed(string message = null) =>
            new Result<T>(messages: message);
    }
}