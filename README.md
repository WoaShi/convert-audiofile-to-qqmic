# 使用方法

## 重要说明

**软件需求**：

    该软件赖虚拟声卡（例如 `VoiceMeeter`）。

**兼容性**：该软件支持如 QQNT 等通过按下语音按钮发送语音的软件。

## 食用步骤

0. 有关虚拟声卡，以VoiceMeeter举例，在VoiceMeeter初始化完成后，不需要在使用本软件的同时运行VoiceMeeter。

1. 打开准备发送语音的聊天窗口，并打开语音输入界面，例如下图。关于窗口名称，举例：该QQ群聊窗口“核实验场”会在软件中显示为“QQ”。

   ![说明](https://github.com/user-attachments/assets/6bf3b043-83d6-46e1-8680-268ee03fd013)
  
3. 打开软件本体。
  
4. 点击 `选择音频文件` 按钮，在电脑中选择想要转录的音频。
  
  ![说明](https://github.com/user-attachments/assets/d47ad4dd-f5f8-47b3-9804-c61e1ed40284)

  * 图中 1、2 都能进行交互，选取成功后，右侧方框会显示音频路径。
4. 点击 `选择聊天窗口` 按钮，打开选择QQ聊天窗口界面：
  
  * 选择与刚才聊天窗口名称相匹配的窗口（如图中的 “QQ”），点击确认。![bf7ff5ec-5659-495a-8572-04c2191f2ea9](https://github.com/user-attachments/assets/d0dc655d-e2bc-4635-9e82-8126c5164f7d)
  * **注**：若"QQ"未出现在列表中，请点开聊天窗口使其出现在屏幕中，然后点击 `刷新`，再次在列表中寻找"QQ"。
5. 若选择成功，右侧方框将显示窗口名称。
  
6. 点击 `选择虚拟声卡` 栏目选取你的虚拟声卡输入，确保其与虚拟声卡输出相匹配，例如 VoiceMeeter VAIO3 Input 对应的麦克风输入应设置为 VoiceMeeter VAIO3 Output。
  
7. 点击 `截图语音按钮`，对语音按钮进行裁截，如图。
  
  ![5c644045-93eb-4f73-a9b4-addb09fdbb22](https://github.com/user-attachments/assets/c8c1fbb5-4053-41c9-a418-ec995f0f40bf)
  
8. 若报错“图片错误！”，可自行将语音按钮截图保存到软件同目录的 `Images` 文件夹下，并重命名为 `MicButton.png`。
  
9. 正常来说，到这一步，你已经完成了所有步骤，点击 `播放到QQ有语音`即可开始转录。
  
  * **注意**：由于QQNT的问题，若你在选择好虚拟声卡后点击 `播放到语音软件`，发现QQ的语音没有声音，建议将虚拟声卡的输出质量在`44100Hz`/`48000Hz`/`8000Hz`间变动。
  * **注意**：请自行在Windows设置QQ输入麦克风为虚拟声卡的Output端，如图。
  * **注意**：若点击`播放到语音软件`后，若鼠标锁定在了其他圆形元素上，可以适当对图像识别置信度进行调节。

## 鸣谢

FluentUI库：[GitHub - iNKORE-NET/UI.WPF.Modern: Modern (Fluent 2) styles and controls for your WPF applications](https://github.com/iNKORE-NET/UI.WPF.Modern)

音频工具：[GitHub - naudio/NAudio: Audio and MIDI library for .NET](https://github.com/naudio/NAudio)

                    [GitHub - BunLabs/NAudio.Flac: A FLAC library for NAudio ≥ 2.0](https://github.com/BunLabs/NAudio.Flac)

图像识别：OpenCvSharp

十分感谢 ETO-QSH 对该软件的建议与修正。
