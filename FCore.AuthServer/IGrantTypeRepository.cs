using FCore.Foundations;

namespace FCore.AuthServer
{
    public interface IGrantTypeRepository
    {
       /// <summary>
       /// Return a specific grant type (though it will be treated like any grant type). It will return null
       /// if it doesn't exist.
       /// </summary>
       /// <param name="grantTypeName"></param>
       /// <returns></returns>
       GrantType GetGrantType(string grantTypeName);
    }
}
