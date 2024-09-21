# ���÷�������ַ�Ͷ˿�
$serverAddress = "127.0.0.1"
$serverPort = 9999

# ����һ�� TcpClient ʵ��
$client = New-Object System.Net.Sockets.TcpClient

try {
    # ���ӵ�������
    $client.Connect($serverAddress, $serverPort)
    Write-Host "Connected to server."

    # ��ȡ������
    $networkStream = $client.GetStream()

    # ���ö�д��ʱʱ��
    $networkStream.ReadTimeout = 10000
    $networkStream.WriteTimeout = 10000

    # ѭ��������Ϣ
    while ($true) {
        # Ҫ���͵���Ϣ
        $message = "Hello, Server!"
        $messageBytes = [System.Text.Encoding]::ASCII.GetBytes($message + "`r`n")  # Telnet Э��ͨ��ʹ�� CRLF ��Ϊ�н�����

        # ������Ϣ
        $networkStream.Write($messageBytes, 0, $messageBytes.Length)
        Write-Host "Message sent."

        # ��ȡ��������Ӧ
        if ($networkStream.DataAvailable) {
            $buffer = New-Object byte[] 1024
            $readBytes = $networkStream.Read($buffer, 0, $buffer.Length)
            if ($readBytes -gt 0) {
                $response = [System.Text.Encoding]::ASCII.GetString($buffer, 0, $readBytes)
                Write-Host "Received response: $response"
            }
        }

        # ��ͣ1��
        Start-Sleep -Seconds 1
    }

    # �ر��������Ϳͻ���
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