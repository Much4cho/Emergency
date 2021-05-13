package com.example.consumer;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.ConfigurableApplicationContext;
import org.springframework.context.annotation.Bean;
import org.springframework.kafka.annotation.KafkaListener;

import java.util.concurrent.CountDownLatch;
import java.util.concurrent.TimeUnit;

@SpringBootApplication
public class ConsumerApplication {

    public static void main(String[] args) throws Exception {

        ConfigurableApplicationContext context = SpringApplication.run(ConsumerApplication.class, args);

        MessageListener listener = context.getBean(MessageListener.class);
        listener.latch.await(3600, TimeUnit.SECONDS);
    }

    @Bean
    public MessageListener messageListener() {
        return new MessageListener();
    }

    public static class MessageListener {

        private CountDownLatch latch = new CountDownLatch(10);

        @KafkaListener(topics = "topic")
        public void listen(String message) {
            System.out.println("Received Message: " + message);
            latch.countDown();
        }

    }
}
