# ���÷�������ַ�Ͷ˿�
$serverAddress = "127.0.0.1"
$serverPort = 9999
$message = $args[0]

if ($null -eq $message -or $message -eq "") {
    Write-Output "cmd is empty or null"
	return
} else {
    Write-Output "cmd: $message "
}

# ����һ�� Socket ʵ��
$addressFamily = [System.Net.Sockets.AddressFamily]::InterNetwork
$socketType = [System.Net.Sockets.SocketType]::Stream
$protocolType = [System.Net.Sockets.ProtocolType]::Tcp
$socket = New-Object System.Net.Sockets.Socket($addressFamily, $socketType, $protocolType)

try {
    # ������������ַ
    $ipAddress = [System.Net.IPAddress]::Parse($serverAddress)
    $remoteEndPoint = New-Object System.Net.IPEndPoint($ipAddress, $serverPort)
    
    # ���ӵ�������
    $socket.Connect($remoteEndPoint)
    Write-Host "Connected to server."

    # Ҫ���͵���Ϣ
    $messageBytes = [System.Text.Encoding]::ASCII.GetBytes($message)

    # ������Ϣ
    $socket.Send($messageBytes)
    Write-Host "Message sent."

    # �ر��׽���
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