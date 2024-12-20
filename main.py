import pygame
import random


pygame.init()


WIDTH, HEIGHT = 800, 600
WHITE = (255, 255, 255)
BLACK = (0, 0, 0)
RED = (255, 0, 0)
FPS = 60


screen = pygame.display.set_mode((WIDTH, HEIGHT))
pygame.display.set_caption("Избегай препятствий!")


class Player:
    def __init__(self):
        self.width = 50
        self.height = 50
        self.x = WIDTH // 2 - self.width // 2
        self.y = HEIGHT - self.height - 10
        self.speed = 5

    def move(self, dx):
        self.x += dx

        if self.x < 0:
            self.x = 0
        if self.x > WIDTH - self.width:
            self.x = WIDTH - self.width

    def draw(self, surface):
        pygame.draw.rect(surface, BLACK, (self.x, self.y, self.width, self.height))


class Obstacle:
    def __init__(self):
        self.width = random.randint(20, 100)
        self.height = random.randint(20, 50)
        self.x = random.randint(0, WIDTH - self.width)
        self.y = -self.height
        self.speed = random.randint(3, 7)

    def fall(self):
        self.y += self.speed

    def draw(self, surface):
        pygame.draw.rect(surface, RED, (self.x, self.y, self.width, self.height))

    def off_screen(self):
        return self.y > HEIGHT


def main():
    clock = pygame.time.Clock()
    player = Player()
    obstacles = []
    score = 0
    running = True

    while running:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False

        keys = pygame.key.get_pressed()
        if keys[pygame.K_LEFT]:
            player.move(-player.speed)
        if keys[pygame.K_RIGHT]:
            player.move(player.speed)


        if random.randint(1, 30) == 1:
            obstacles.append(Obstacle())


        for obstacle in obstacles:
            obstacle.fall()
            if obstacle.off_screen():
                obstacles.remove(obstacle)
                score += 1


        for obstacle in obstacles:
            if (player.x < obstacle.x + obstacle.width and
                player.x + player.width > obstacle.x and
                player.y < obstacle.y + obstacle.height and
                player.y + player.height > obstacle.y):
                print(f"Игра окончена! Ваш счет: {score}")
                running = False


        screen.fill(WHITE)
        player.draw(screen)
        for obstacle in obstacles:
            obstacle.draw(screen)


        font = pygame.font.SysFont(None, 36)
        score_text = font.render(f'Счет: {score}', True, BLACK)
        screen.blit(score_text, (10, 10))

        pygame.display.flip()
        clock.tick(FPS)

    pygame.quit()

if __name__ == "__main__":
    main()
