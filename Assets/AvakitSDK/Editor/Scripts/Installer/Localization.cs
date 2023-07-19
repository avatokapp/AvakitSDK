using System;

namespace AvakitSDK.Installer
{
    public static class Localization
    {
        public enum Languages
        {
            eng,
            kor,
            jap,
        }

        public static Languages Lang = Languages.eng;

        public static string RequiredMessage(string name)
        {
            switch (Lang)
            {
                case Languages.jap:
                    return $"必須項目。\"{name}\" を入力してください";
                case Languages.eng:
                    return $"\"{name}\" is required";
                case Languages.kor:
                    return $"\"{name}\" 이(가) 필요합니다";
                default:
                    throw new NotImplementedException();
            }
        }

        public static string InvalidComponentMessage(string name)
        {
            switch (Lang)
            {
                case Languages.jap:
                    return $"\"{name}\" は無効なコンポーネントです";
                case Languages.eng:
                    return $"\"{name}\" is invalid component";
                case Languages.kor:
                    return $"\"{name}\" 은(는) 유효하지 않은 컴포넌트입니다";
                default:
                    throw new NotImplementedException();
            }
        }

        public static string DegradePerformanceMessage(string name)
        {   
            switch (Lang)
            {
                case Languages.jap:
                    return $"\"{name}\" はパフォーマンスを低下させる可能性があります";
                case Languages.eng:
                    return $"\"{name}\" may degrade performance";
                case Languages.kor:
                    return $"\"{name}\" 은(는) 성능을 저하시킬 수 있습니다";
                default:
                    throw new NotImplementedException();
            }
        }

        public static string NotSupportedMessage(string name)
        {
            switch (Lang)
            {
                case Languages.jap:
                    return $"\"{name}\" はサポートされていません";
                case Languages.eng:
                    return $"\"{name}\" is not supported";
                case Languages.kor:
                    return $"\"{name}\" 은(는) 지원되지 않습니다";
                default:
                    throw new NotImplementedException();
            }
        }

        #region Words

        public static class Words
        {
            public static string Language => LanguageWords[(int)Lang];
            public static string Close => CloseWords[(int)Lang];
            public static string InstallAll => InstallAllWords[(int)Lang];
            public static string UnityVersionNotice => UnityVersionNoticeWords[(int)Lang];
            public static string PackageNotice => PackageNoticeWords[(int)Lang];
            public static string ShaderNotice => ShaderNoticeWords[(int)Lang];
            public static string Discord => DiscordWords[(int)Lang];
            
            private static string[] LanguageWords = { "Language", "언어", "言語" };
            private static string[] CloseWords = { "Close", "닫기" ,"削除" };
            private static string[] InstallAllWords = { "Install All", "모두 설치" ,"すべてインストール" };
            private static string[] UnityVersionNoticeWords = { "Current version of unity editor is not supported. You need to use editor between Unity 2021.3.1f1 to 2021.3.8f1, and we recommend you to use 2021.3.3f1.", "지원되지 않는 버전의 유니티 에디터를 사용 중입니다. 2021.3.1f1에서 2021.3.8f1 사이에 에디터를 사용해야 하며, 2021.3.3f1 사용을 권장합니다.", "Unity エディターの現在のバージョンはサポートされていません。 Unity 2021.3.1f1 から 2021.3.8f1 までのエディターを使用する必要があり、2021.3.3f1 の使用を推奨します。" };
            private static string[] PackageNoticeWords = { "Required Package", "필요한 패키지", "インストールが必要なパッケージ" };
            private static string[] ShaderNoticeWords = { "Supported Shaders", "지원되는 셰이더", "サポートされているシェーダー"};
            private static string[] DiscordWords = { "Discord", "디스코드", "ディスコード" };
        }

        #endregion
    }
}