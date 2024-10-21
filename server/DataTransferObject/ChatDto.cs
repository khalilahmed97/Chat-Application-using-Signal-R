namespace server.DataTransferObject
{
    public sealed record ChatDto
        (
        Guid UserId,
        Guid ToUserId,
        string message
        );
}


