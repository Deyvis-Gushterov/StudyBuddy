
namespace StudyBuddy.Common
{
    public class ValidationConstants
    {
        //User
        public const int MinNameLength = 2;
        public const int MaxNameLength = 90;

        //Note
        public const int MinTopicLength = 3;
        public const int MaxTopicLength = 100;
        public const int MinExplanationLength = 20;
        public const int MaxExplanationLength = 200;
        public const int MinNoteLength = 200;

        //Blog
        public const int MinBlogTitleLength = 3;
        public const int MaxBlogTitleLength = 100;
        public const int MinContentLength = 100;
        public const int MinSummaryLength = 50;

        //Comment
        public const int MinCommentContentLength = 1;
        public const int MaxCommentContentLength = 300;

        //Report
        public const int MaxReportReasonLength = 90;
        public const int MinReportDetailsLength = 10;
        public const int MaxReportDetailsLength = 500;

        //Message
        public const int MinFeedbackMessageLength = 10;
        public const int MaxFeedbackMessageLength = 200;

        //StudyGroup
        public const int MaxGroupNameLength = 60;
        public const int MinGroupDescriptionLength = 10;
        public const int MaxGroupDescriptionLength = 200;
    }
}
