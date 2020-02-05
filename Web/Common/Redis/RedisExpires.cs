/// <summary>
/// Redis 有效时间（单位：分钟）定义类。
/// </summary>
public static class RedisExpires
{
    /// <summary>
    /// 1 分钟。
    /// </summary>
    public static readonly int ONE_MINUTE = 1;
 
    /// <summary>
    /// 10 分钟。
    /// </summary>
    public static readonly int TEN_MINUTE = 10;
 
    /// <summary>
    /// 半小时。
    /// </summary>
    public static readonly int HALF_HOUR = 30;
 
    /// <summary>
    /// 1 小时。
    /// </summary>
    public static readonly int ONE_HOUR = 60;
 
    /// <summary>
    /// 半天。
    /// </summary>
    public static readonly int HALF_DAY = 60 * 12;
 
    /// <summary>
    /// 1 天。
    /// </summary>
    public static readonly int ONE_DAY = 60 * 24;
 
    /// <summary>
    /// 1 周。
    /// </summary>
    public static readonly int ONE_WEEK = 7 * 60 * 24;
 
    /// <summary>
    /// 1 个月。
    /// </summary>
    public static readonly int ONE_MONTH = 30 * 60 * 24;
}