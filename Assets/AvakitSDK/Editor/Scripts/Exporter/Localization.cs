using System;

namespace AvakitSDK.Exporter
{
    public static class Localization
    {
        public enum Languages
        {
            eng,
            kor,
            jpn,
        }

        public static Languages Lang = Languages.eng;

        public static string RequiredMessage(string name)
        {
            switch (Lang)
            {
                case Languages.jpn:
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
                case Languages.jpn:
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
                case Languages.jpn:
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
                case Languages.jpn:
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
            public static string Delete => DeleteWords[(int)Lang];
            public static string DeleteAll => DeleteAllWords[(int)Lang];
            public static string Resolve => ResolveWords[(int)Lang];
            public static string ResolveAll => ResolveAllWords[(int)Lang];
            public static string RootCheck => RootCheckWords[(int)Lang];
            public static string ComponentCheck => ComponentCheckWords[(int)Lang];
            public static string MaterialCheck => MaterialCheckWords[(int)Lang];
            public static string Export => ExportWords[(int)Lang];
            public static string ComponentNotice => ComponentNoticeWords[(int)Lang];
            public static string ShaderNotice => ShaderNoticeWords[(int)Lang];
            
            private static string[] LanguageWords = { "Language", "언어", "言語" };
            private static string[] DeleteWords = { "Delete", "삭제" ,"削除" };
            private static string[] DeleteAllWords = { "Delete All", "모두 삭제" ,"すべて削除" };
            private static string[] ResolveWords = { "Resolve", "변경" ,"解決" };
            private static string[] ResolveAllWords = { "Resolve All", "모두 변경" ,"すべて解決" };
            private static string[] RootCheckWords = { "Root Check", "Root 확인" ,"Root チェック" };
            private static string[] ComponentCheckWords = { "Component Check", "Component 확인" ,"Component チェック" };
            private static string[] MaterialCheckWords = { "Material Check", "Material 확인" ,"Material チェック" };
            private static string[] ExportWords = { "Export", "내보내기", "エクスポート" };
            private static string[] ComponentNoticeWords = { "You can add a Particle System using the basic Shader provided by Unity. Use it to create your own effect assets.", "유니티에서 제공하는 기본 Shader을 이용한 Particle System을 추가할 수 있습니다. 이를 활용하여 나만의 효과 에셋을 만들어보세요.", "Unityが提供するネイティブShaderを使用したParticle Systemを追加できます。これを活用して独自のエフェクトアセットを作成してください。" };
            private static string[] ShaderNoticeWords = { "Even if there is no problem with exporting, some shaders may be changed to the default shader (lilToon) when used in avakit.", "내보내기에 문제가 없더라도, avakit에서 이용 시 일부 shader가 default shader(lilToon)으로 변경될 수 있습니다.", "エクスポートに問題がなくても、avakitで使用すると一部のシェーダがデフォルトシェーダ（lilToon）に変更されることがあります。"};
        }

        #endregion
    }
}