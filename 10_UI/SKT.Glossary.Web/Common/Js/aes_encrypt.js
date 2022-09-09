/*
Author : 개발자- 최현미C, 리뷰자-진현빈D
CreateDae :  2016.04.21
Desc : 파라미터 암호화 CHOI    
*/
function EncryptParam(param) {

    var key = CryptoJS.enc.Utf8.parse('8080808080808080');
    var iv = CryptoJS.enc.Utf8.parse('8080808080808080');
    var tmpWeeklyID = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(param), key,
    {
        keySize: 128 / 8,
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
    });

    return tmpWeeklyID;
}