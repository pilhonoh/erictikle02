﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
<<<<<<< HEAD
//     런타임 버전:4.0.30319.34209
=======
//     런타임 버전:4.0.30319.42000
>>>>>>> 58671c0c20e24bdacd11e4e4a4c54898af7ff265
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SKT.Tnet.Framework.Properties {
    using System;
    
    
    /// <summary>
    ///   지역화된 문자열 등을 찾기 위한 강력한 형식의 리소스 클래스입니다.
    /// </summary>
    // 이 클래스는 ResGen 또는 Visual Studio와 같은 도구를 통해 StronglyTypedResourceBuilder
    // 클래스에서 자동으로 생성되었습니다.
    // 멤버를 추가하거나 제거하려면 .ResX 파일을 편집한 다음 /str 옵션을 사용하여 ResGen을
    // 다시 실행하거나 VS 프로젝트를 다시 빌드하십시오.
<<<<<<< HEAD
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
=======
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
>>>>>>> 58671c0c20e24bdacd11e4e4a4c54898af7ff265
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   이 클래스에서 사용하는 캐시된 ResourceManager 인스턴스를 반환합니다.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SKT.Tnet.Framework.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
<<<<<<< HEAD
        ///   이 강력한 형식의 리소스 클래스를 사용하여 모든 리소스 조회에 대한 현재 스레드의 CurrentUICulture
        ///   속성을 재정의합니다.
=======
        ///   이 강력한 형식의 리소스 클래스를 사용하여 모든 리소스 조회에 대해 현재 스레드의 CurrentUICulture 속성을
        ///   재정의합니다.
>>>>>>> 58671c0c20e24bdacd11e4e4a4c54898af7ff265
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Command Text is null.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string CommandTextNullException {
            get {
                return ResourceManager.GetString("CommandTextNullException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Section of the component that reads the error occurred. (DeserializeSection)과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string ConfigDeserializeException {
            get {
                return ResourceManager.GetString("ConfigDeserializeException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   섹션 정보를 가져오는 중 오류가 발생하였습니다.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string ConfigSectionException {
            get {
                return ResourceManager.GetString("ConfigSectionException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Exception occured in ExecuteDataSet Method.  ({0}:{1})과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string ExecuteDataSetException {
            get {
                return ResourceManager.GetString("ExecuteDataSetException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Exception occured in ExecuteNonQuery Method. ({0}:{1})과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string ExecuteNonQueryException {
            get {
                return ResourceManager.GetString("ExecuteNonQueryException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Exception occured in ExecuteNonQuery Method. ({0}:{1})과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string ExecuteNonQueryException1 {
            get {
                return ResourceManager.GetString("ExecuteNonQueryException1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Exception occured in ExecuteReader Method. ({0}:{1})과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string ExecuteReaderException {
            get {
                return ResourceManager.GetString("ExecuteReaderException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Exception occured in ExecuteScalar Method. ({0}:{1})과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string ExecuteScalarException {
            get {
                return ResourceManager.GetString("ExecuteScalarException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Exception occured in ExecuteXmlReader Method. ({0}:{1})과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string ExecuteXmlReaderException {
            get {
                return ResourceManager.GetString("ExecuteXmlReaderException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Exception  occured in Message Reading.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string GetMessageException {
            get {
                return ResourceManager.GetString("GetMessageException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   {0} 값이 null입니다.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string HttpContexNullException {
            get {
                return ResourceManager.GetString("HttpContexNullException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Message Table is null.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string MessageTableNullException {
            get {
                return ResourceManager.GetString("MessageTableNullException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   DataBase Access 하기 위한 초기 설정이 되지 않았습니다.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string NoInitSettingForDBAccess {
            get {
                return ResourceManager.GetString("NoInitSettingForDBAccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   {0} 섹션에 {1} 카테고리 또는 {2} 키가 정의되지 않았습니다.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string NotFoundCategoryOrKeyInConfigSection {
            get {
                return ResourceManager.GetString("NotFoundCategoryOrKeyInConfigSection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   {0} 섹션이 정의되지 않았습니다.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string NotFoundConfigSection {
            get {
                return ResourceManager.GetString("NotFoundConfigSection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Exception occured in Sql Bulk Copy . (TableName:{0})과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string SqlbulkCopyException {
            get {
                return ResourceManager.GetString("SqlbulkCopyException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   SqlCommand is null.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string SqlCommandNullException {
            get {
                return ResourceManager.GetString("SqlCommandNullException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Transaction was rollbacked or commited, please provide an open transaction.과(와) 유사한 지역화된 문자열을 찾습니다.
        /// </summary>
        internal static string TransactionConnectionNullException {
            get {
                return ResourceManager.GetString("TransactionConnectionNullException", resourceCulture);
            }
        }
    }
}
