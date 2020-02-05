/// <summary>
/// 全局 Key 定义。
/// </summary>
public static class RedisKeys
{
 
    #region 广告相关...
 
    /// <summary>
    /// 指定城市唯一编号指定唯一广告编号的信息实体对象，{0}：指定城市唯一编号，{1}：指定广告编码。
    /// </summary>
    public static readonly string ADVERTISEMENTSEAT_CITY_BAIDUADID = "AdvertisementSeat_City_{0}_BaiDuAdId_{1}";
    /// <summary>
    /// 指定城市唯一编号指定广告投放目标的信息实体对象，{0}：指定城市唯一编号，{1}：指定广告投放目标。
    /// </summary>
    public static readonly string ADVERTISEMENTSEAT_CITY_BAIDUADID_TARGET = "AdvertisementSeat_City_{0}_Target_{1}";
    /// <summary>
    /// 指定城市唯一编号指定唯一广告编号的信息实体对象，{0}：指定城市唯一编号，{1}：指定产品类型编码。
    /// </summary>
    public static readonly string ADVERTISEMENTSEAT_CITY_CONDITION = "AdvertisementSeat_City_{0}_Condition_{1}";
 
    #endregion
 
}