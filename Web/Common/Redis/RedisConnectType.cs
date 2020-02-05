/// <summary>
/// 定义 Redis 建立连接的方式，连接池模式，及时连接及时释放模式。
/// </summary>
public enum RedisConnectType
{
    /// <summary>
    /// 连接池链接方式。
    /// </summary>
    PooledRedisClient,
    /// <summary>
    /// 短连接方式，用完就释放的模式。
    /// </summary>
    ShortConnectClient
}