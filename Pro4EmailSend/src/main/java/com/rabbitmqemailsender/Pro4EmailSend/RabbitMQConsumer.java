package com.rabbitmqemailsender.Pro4EmailSend;


import jakarta.mail.MessagingException;
import jakarta.mail.internet.MimeMessage;
import org.springframework.amqp.core.Message;
import org.springframework.amqp.core.MessageListener;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.mail.javamail.JavaMailSender;
import org.springframework.mail.javamail.MimeMessageHelper;

import freemarker.template.Configuration;
import freemarker.template.Template;
import freemarker.template.TemplateException;

import org.springframework.stereotype.Service;
import org.springframework.ui.freemarker.FreeMarkerTemplateUtils;

import java.io.IOException;
import java.util.HashMap;
import java.util.Map;

@Service
public class RabbitMQConsumer implements MessageListener {
    private final ObjectMapper objectMapper = new ObjectMapper();

    @Autowired
    private JavaMailSender mailSender;

    @Autowired
    private Configuration freemarkerConfig;

    public void onMessage(Message message) {
        try {
            EmailAuthenDTO emailAuthenDTO = objectMapper.readValue(message.getBody(), EmailAuthenDTO.class);
            System.out.println("Received email for authentication: " + emailAuthenDTO.Email);
            System.out.println("Authentication URL: " + emailAuthenDTO.Url);

            sendEmail(emailAuthenDTO);

        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private void sendEmail(EmailAuthenDTO emailAuthenDTO) throws MessagingException, IOException, TemplateException {
        Template template = freemarkerConfig.getTemplate("email_verification_template.ftl");

        Map<String, Object> model = new HashMap<>();
        model.put("email", emailAuthenDTO.Email);
        model.put("url", emailAuthenDTO.Url);

        String htmlContent = FreeMarkerTemplateUtils.processTemplateIntoString(template, model);

        MimeMessage message = mailSender.createMimeMessage();
        MimeMessageHelper helper = new MimeMessageHelper(message, true, "UTF-8");

        helper.setTo(emailAuthenDTO.Email);
        helper.setSubject("Email Authentication");
        helper.setText(htmlContent, true);

        mailSender.send(message);
        System.out.println("Email sent successfully to: " + emailAuthenDTO.Email);
    }
}
