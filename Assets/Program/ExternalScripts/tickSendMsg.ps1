# 设置服务器地址和端口
$serverAddress = "127.0.0.1"
$serverPort = 9999

# 创建一个 TcpClient 实例
$client = New-Object System.Net.Sockets.TcpClient

try {
    # 连接到服务器
    $client.Connect($serverAddress, $serverPort)
    Write-Host "Connected to server."

    # 获取网络流
    $networkStream = $client.GetStream()

    # 设置读写超时时间
    $networkStream.ReadTimeout = 10000
    $networkStream.WriteTimeout = 10000

    # 循环发送消息
    while ($true) {
        # 要发送的消息
        $message = "Hello, Server!"
        $messageBytes = [System.Text.Encoding]::ASCII.GetBytes($message + "`r`n")  # Telnet 协议通常使用 CRLF 作为行结束符

        # 发送消息
        $networkStream.Write($messageBytes, 0, $messageBytes.Length)
        Write-Host "Message sent."

        # 读取服务器响应
        if ($networkStream.DataAvailable) {
            $buffer = New-Object byte[] 1024
            $readBytes = $networkStream.Read($buffer, 0, $buffer.Length)
            if ($readBytes -gt 0) {
                $response = [System.Text.Encoding]::ASCII.GetString($buffer, 0, $readBytes)
                Write-Host "Received response: $response"
            }
        }

        # 暂停1秒
        Start-Sleep -Seconds 1
    }

    # 关闭网络流和客户端
    $networkStream.Close()
    $client.Close()
    Write-Host "Disconnected from server."
}
catch {
    Write-Host "An error occurred: $_"
    if ($networkStream -ne $null) {
        $networkStream.Close()
    }
    if ($client.Connected) {
        $client.Close()
    }
}