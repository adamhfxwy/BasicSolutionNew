namespace Solution.Core.DTO.Common.BaiDu
{
    public class SpeechResponseDTO
    {
        public string? corpus_no { get; set; }
        public string? err_msg { get; set; }
        public string? err_no { get; set; }
        public List<string> result {  get; set; }
        public string? sn { get; set; }
    }
}
