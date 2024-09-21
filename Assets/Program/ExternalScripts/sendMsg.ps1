# 设置服务器地址和端口
$serverAddress = "127.0.0.1"
$serverPort = 9999
$message = $args[0]

if ($null -eq $message -or $message -eq "") {
    Write-Output "cmd is empty or null"
	return
} else {
    Write-Output "cmd: $message "
}

# 创建一个 Socket 实例
$addressFamily = [System.Net.Sockets.AddressFamily]::InterNetwork
$socketType = [System.Net.Sockets.SocketType]::Stream
$protocolType = [System.Net.Sockets.ProtocolType]::Tcp
$socket = New-Object System.Net.Sockets.Socket($addressFamily, $socketType, $protocolType)

try {
    # 解析服务器地址
    $ipAddress = [System.Net.IPAddress]::Parse($serverAddress)
    $remoteEndPoint = New-Object System.Net.IPEndPoint($ipAddress, $serverPort)
    
    # 连接到服务器
    $socket.Connect($remoteEndPoint)
    Write-Host "Connected to server."

    # 要发送的消息
    $messageBytes = [System.Text.Encoding]::ASCII.GetBytes($message)

    # 发送消息
    $socket.Send($messageBytes)
    Write-Host "Message sent."

    # 关闭套接字
    $socket.Shutdown([System.Net.Sockets.SocketShutdown]::Both)
    $socket.Close()
    Write-Host "Socket closed."
}
catch {
    Write-Host "An error occurred: $_"
    if ($socket -ne $null -and $socket.Connected) {
        $socket.Shutdown([System.Net.Sockets.SocketShutdown]::Both)
        $socket.Close()
    }
}