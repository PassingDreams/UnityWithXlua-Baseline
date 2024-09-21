# Define server and port
$server = "127.0.0.1"  # Replace with the server's IP address or hostname
$port = 9999         # Replace with the server's port number

try {
    # Create a new TCP client and connect to the server
    $client = New-Object System.Net.Sockets.TcpClient
    $client.Connect($server, $port)

    # Get the network stream for reading and writing
    $networkStream = $client.GetStream()

    # Create a StreamWriter for sending data
    $writer = New-Object System.IO.StreamWriter($networkStream)
    $writer.AutoFlush = $true

    # Create a StreamReader for receiving data
    $reader = New-Object System.IO.StreamReader($networkStream)

    Write-Output "Connected to $server on port $port. Type your message and press Enter to send."

    while ($true) {
        # Check if there is data available to read
        if ($networkStream.DataAvailable) {
            $response = $reader.ReadLine()
            Write-Output "Server: $response"
        }

        # Read user input
        $message = Read-Host

        # Send the message to the server
        $writer.WriteLine($message)

        # Exit loop if user types 'exit'
        if ($message -eq "exit") {
            break
        }
    }
} catch {
    Write-Error "An error occurred: $_"
} finally {
    # Cleanup
    if ($client.Connected) {
        $writer.Close()
        $reader.Close()
        $client.Close()
        Write-Output "Disconnected from server."
    }
}
