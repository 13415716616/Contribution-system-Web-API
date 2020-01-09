using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Contribution_system_Commond
{
    static public class UserCommond
    {
        public static string GetMD5Hash(string str)
        {
            //就是比string往后一直加要好的优化容器
            System.Text.StringBuilder sb = new StringBuilder();
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                //将输入字符串转换为字节数组并计算哈希。
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                //X为     十六进制 X都是大写 x都为小写
                //2为 每次都是两位数
                //假设有两个数10和26，正常情况十六进制显示0xA、0x1A，这样看起来不整齐，为了好看，可以指定"X2"，这样显示出来就是：0x0A、0x1A。 
                //遍历哈希数据的每个字节
                //并将每个字符串格式化为十六进制字符串。
                int length = data.Length;
                for (int i = 0; i < length; i++)
                    sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }

        static public string SetToken(string username,string Role)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name,username),
                new Claim(ClaimTypes.Role,Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qpb9Ao1VxCjWAXWM25aHaCwVVYX7bZX3"));
            var token = new JwtSecurityToken(
                issuer: "Contribution-system_Asp_Net_Core",  //颁发者
                audience: "Contribution-system_Vue", //接收者
                claims: claims,   //身份数据
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(23),   //Token持续时间
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }
    }
}
