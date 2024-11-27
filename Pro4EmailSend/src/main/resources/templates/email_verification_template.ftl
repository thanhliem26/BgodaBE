<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Xác minh email của bạn</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 20px;
            background-color: #f4f4f4;
            color: #333;
        }
        .container {
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            background-color: #ffffff;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }
        h1 {
            color: #2d72fc;
        }
        .button {
            display: inline-block;
            padding: 12px 20px;
            margin: 20px 0;
            color: #ffffff; /* Màu chữ trong button */
            background-color: #2d72fc;
            border: none;
            border-radius: 4px;
            text-decoration: none;
            font-weight: bold;
            text-align: center;
        }
        .footer {
            margin-top: 20px;
            font-size: 0.9em;
            color: #777;
        }
    </style>
</head>
<body>
<div class="container">
    <h1>Xác minh email của bạn</h1>
    <p>Xin chào, <strong>${email}</strong>!</p>

    <p>Cảm ơn bạn đã đăng ký! Chúng tôi chỉ cần bạn xác minh địa chỉ email của mình để hoàn tất quá trình đăng ký.</p>

    <p>Vui lòng nhấp vào nút dưới đây để xác minh địa chỉ email của bạn:</p>

    <form id="verificationForm" action="${url}" method="POST">
        <input type="hidden" name="email" value="${email}">
        <button type="submit" class="button">Xác minh email</button>
    </form>

    <p>Nếu bạn không yêu cầu đăng ký tài khoản này, vui lòng bỏ qua email này.</p>

    <p class="footer">Trân trọng,<br>Đội ngũ hỗ trợ của bạn</p>
</div>
</body>
</html>
