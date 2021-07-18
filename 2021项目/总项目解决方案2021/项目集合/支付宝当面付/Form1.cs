using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using Alipay.EasySDK.Factory;
using Alipay.EasySDK.Kernel;
using Alipay.EasySDK.Kernel.Util;
using Alipay.EasySDK.Payment.FaceToFace.Models;

namespace 支付宝当面付
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        static private Config GetConfig()
            {
            return new Config()
            {
                Protocol = "https",
                GatewayHost = "openapi.alipay.com",
                SignType = "RSA2",

                AppId = "2021002158651326",

                // 为避免私钥随源码泄露，推荐从文件中读取私钥字符串而不是写入源码中
                MerchantPrivateKey = "MIIEpAIBAAKCAQEAmknVXrpYrCJYepGNOzOAqrKrKXgwHKzzdDlwhU5BE8k1kkLD7NyvlEzHVCAFFAGux2WafAGi9lK3GGAD9Lruu56x9D0WJ1Bis4YRY2RzuxGVamVS7Oav0PBFqDeD8zb2okhVedhYuUz6xgtxZg5eeuaergspXQ0DNGbYlrj1lDtZdfHMZ6eP5WOe7nWUOaLvpqJH4f5tnOsxafyE1jkmJYds6hM6OJMX3PqYHeXH5cfjuLrpLap1uZnjs9ZSCWQ8s8CR8lf+An5DFII9ntrYwVOxFNx57AZu4lhbnu7sAhfnrXYlh2EKIim+M1NGLQ6J2bfbaXMLDAnm+eHuWCx0jwIDAQABAoIBAGafa4upeOdtpNpJy92vwQpI8u4PYjkAlKIevof8Z+7IK5jQTc9Tbnm+o+qBrb8D64P3QczvrbwXgm91FcyHNdmXkZf5ta2Km0v7hb9NhmjMJkzxfjnSqujXmA1ud5ajXWLNqAT2cPU3jamC3Pdb4V3v3WW4SNf6msIVEkUWW0ovMzG1qkdlMG2+Qxs2H04rPJvTUGOTKJOH3Ayg5t+Jxi/mI4hRA7EpPZ5WSYEYE/ld9tLZ5LVxWr1urXDUyOLVwc3kytxnp4vkhOxbSTZ3ndi27cVbkP3c+SC96Cbn8ikAZk04d7QQUyPvQDfYz9EnbTxdeSDqFZsnup7XWWY24NECgYEAzHPryKAvJfJbeiuK+MHHH/IPaGHtVo6SGE9YF3sdPs+XmmQJWwBClWvQM3+m+wggorQiNdeS2/571AcR7W8+31qQkSVseEfksiqhnbg0caOcWKg490egbusGpSKpVJWTi9zLcBH2qCt2weoHzIL6kSGXUO2Mnte6w4biK7chzwkCgYEAwTAhpu+pLrGHWsbAANAbCHWfui9xL3KO9zEbHmrZoArK81gFrcIqnSRkMx1DmpGc8LV5azp6YSRj6ahNcpBesN38KOSJipUdE31Raen+HF2Q+EkQuuabt9mh0u2pqcPOfqeVD+50BknF6Zw93rklZ9DRZYY8I3r66Bh7cf+b9NcCgYEAhdh9y2HSe+0lpd1LpX52dZtqKtOxJLFBQ1juOrEGfFA614AV/9Uzwc5LBuvSzRSNCPcUTltKcWswdaYLPn1Nk7seWWc+k5+9QEZYd4BYFO0fNYsrf/cMOJD0ULdYU194sF9jb4LTD1Uk8d3cLS6yCsEK5pgdLv/b7JUpC6VgJMECgYAOJjhQwbzibp47R9NIO/W+6N6KBG+Fyh4ufKo/0BOgZSn4KLpBv7bfS4sLM1mAOKoF90StVdsgwkmPE4SYn3pP+fI+DH8GZ2V5x/PujfTOnwu/I5rWFY3SKV5w1HdYt76oCB4izJQy4bRdJ9RgO69Bq0mq5eoBr0AxP5nK/62sgQKBgQCOeokiQkErTI8rQYg5QUkAKL59rN/TF56TNYP3hdyGvfMMl+1/USDwZdpU2J+t83l7yg/ZxBQcgG2GZZZHoXFm9cqa3aYr26kCHD8XYtfzmgu1n+ox4kQ/oCx9JUWZiYl7axnjuGT0xNbxqtmZmIT2BWw3VVwC3gFfiMk6UzK96w==",

                //MerchantCertPath = "<-- 请填写您的应用公钥证书文件路径，例如：/foo/appCertPublicKey_2019051064521003.crt -->",
                //AlipayCertPath = "<-- 请填写您的支付宝公钥证书文件路径，例如：/foo/alipayCertPublicKey_RSA2.crt -->",
                //AlipayRootCertPath = "<-- 请填写您的支付宝根证书文件路径，例如：/foo/alipayRootCert.crt -->",

                // 如果采用非证书模式，则无需赋值上面的三个证书路径，改为赋值如下的支付宝公钥字符串即可
                AlipayPublicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEArVa7JlWYzpbZkTjdrkLzoN3lDsE+A729YNAYTHAFIgVlRfkI6kdOlv+lOVBZnotYateCvYzbo1RC5zrBfrsZzZL5hWWBaIsPrBAV+B7jdlp8xWsko8wp16HH8T6AaNedhVr1U4wq9Zz1nu5Obq9DqXdIZxQxoD1bpmA/C/ZEeOzUiv9OwERtfXQEDgpXGpciy0KAzDzdwiZfWxCzoA7WLCZz5Mz01ADc+0AnaA720rklYzpnM3vU8D8Sr6/kSctNj47y4sy+0Qc3tWhcOQHpca9gz2DlUmBGm3AHh7X4eoPvomNogtdBG3Zt9UA53/HdoGyk7eSAjER/5tW06bxUfwIDAQAB",

                    //可设置异步通知接收服务地址（可选）
                    NotifyUrl = "http://www.acaiji.com/",

                    //可设置AES密钥，调用AES加解密相关接口时需要（可选）
                   // EncryptKey = "<-- 请填写您的AES密钥，例如：aa4BtZ4tspm2wnXLb1ThQA== -->"
                };
            }
  


        private void button1_Click(object sender, EventArgs e)
        {
            // 1. 设置参数（全局只需设置一次）
            Factory.SetOptions(GetConfig());
            try
            {
                // 2. 发起API调用（以创建当面付收款二维码为例）
                AlipayTradePrecreateResponse response = Factory.Payment.FaceToFace()
                    .PreCreate("Apple iPhone11 128G", "2234567234890", "2.00");
                // 3. 处理响应或异常
                if (ResponseChecker.Success(response))
                {
                    MessageBox.Show(response.HttpBody);
                    textBox1.Text = response.QrCode;
                    MessageBox.Show("调用成功");
                }
                else
                {
                    MessageBox.Show("调用失败，原因：" + response.Msg + "，" + response.SubMsg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("调用遭遇异常，原因：" + ex.Message);
                throw ex;
            }
        }




    }
}
