# Sử dụng hình ảnh JDK để build ứng dụng
FROM maven:3.8.4-openjdk-17 AS build

# Thiết lập thư mục làm việc trong container
WORKDIR /app

# Copy toàn bộ mã nguồn vào container
COPY . .

# Build ứng dụng và tạo file .jar
RUN mvn clean package -DskipTests

# Sử dụng JRE nhẹ để chạy ứng dụng sau khi build xong
FROM openjdk:17-jdk-slim

# Thiết lập thư mục làm việc để chứa file .jar đã build
WORKDIR /app

# Copy file .jar từ bước build sang container chạy
COPY --from=build /app/target/*.jar app.jar

# Expose cổng mà Spring Boot sử dụng
EXPOSE 8080

# Khởi chạy ứng dụng
ENTRYPOINT ["java", "-jar", "app.jar"]
