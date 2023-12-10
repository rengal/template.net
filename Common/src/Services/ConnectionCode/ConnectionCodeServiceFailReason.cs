namespace Resto.Common.Services.ConnectionCode
{
    /// <summary>
    /// Причина сбоя сервиса выдачи кодов подключений.
    /// </summary>
    public enum ConnectionCodeServiceFailReason
    {
        None,

        /// <summary>
        /// Код не найден.
        /// </summary>
        CodeNotFound,

        /// <summary>
        /// Код уже был использован.
        /// </summary>
        CodeWasUsed,

        /// <summary>
        /// Сервис на обслуживании.
        /// </summary>
        InternalServiceError,

        /// <summary>
        /// Сервис недоступен.
        /// </summary>
        ServiceUnavailable,

        /// <summary>
        /// Непредвиденная ошибка.
        /// </summary>
        UnexpectedError
    }
}
