using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion; // 必须引入此命名空间

var builder = Kernel.CreateBuilder();

// 1. 配置 GitHub Models 或其他服务
builder.AddOpenAIChatCompletion(
    modelId: "gpt-4o",
    apiKey: "你的GitHub_PAT",
    endpoint: new Uri("https://models.inference.ai.azure.com")
);

var kernel = builder.Build();

// 2. 获取聊天完成服务实例
var chatService = kernel.GetRequiredService<IChatCompletionService>();

// 3. 创建聊天历史记录对象（用于保存上下文）
ChatHistory history = new ChatHistory("你是一个友好的助手。");

Console.WriteLine("机器人已就绪（输入 'exit' 退出）");

while (true)
{
    Console.Write("User > ");
    string? input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input) || input.ToLower() == "exit") break;

    // 将用户输入加入历史
    history.AddUserMessage(input);

    // 4. 发送整个历史记录获取响应
    var response = await chatService.GetChatMessageContentAsync(history);

    // 将 AI 的回复也加入历史
    history.AddAssistantMessage(response.Content!);

    Console.WriteLine($"AI > {response.Content}");
    Console.WriteLine("--------------------------------------------------");
}
