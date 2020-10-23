#include <SFML/Graphics.hpp>
#include <string>

int main() {

    sf::Vector2f screenSize(800, 600);
    int score1 = 0;
    int score2 = 0;
    float velPlayer = 300;
    float velBall = 1.5 * velPlayer;
    sf::Vector2f sizePlayer(30, 150);
    sf::Vector2f sizeBall(sizePlayer.x, sizePlayer.x);
    sf::Vector2f limitsPlayer(screenSize.x - sizePlayer.x, screenSize.y - sizePlayer.y);
    sf::Vector2f limitsBall(screenSize.x - sizeBall.x, screenSize.y - sizeBall.y);
    sf::Vector2f posBall(limitsBall.x / 2, limitsBall.y / 2);
    sf::Vector2f dirBall(velBall, velBall);
    int gapPlayers = 35;
    sf::Vector2f posPlayer1(gapPlayers, limitsPlayer.y / 2);
    sf::Vector2f posPlayer2(limitsPlayer.x - gapPlayers, limitsPlayer.y / 2);
    sf::Vector2f posScorePanel(screenSize.x / 2, 35);

    sf::Font scoreFont;
    scoreFont.loadFromFile("assets/arial.ttf");
    sf::Text scoreText;
    scoreText.setFont(scoreFont);
    scoreText.setStyle(sf::Text::Bold);
    scoreText.setCharacterSize(30);
    scoreText.setFillColor(sf::Color::Red);
    scoreText.setString("");

    sf::RectangleShape player1(sizePlayer);
    sf::RectangleShape player2(sizePlayer);
    sf::RectangleShape ball(sizeBall);
    player1.setFillColor(sf::Color::White);
    player2.setFillColor(sf::Color::White);
    ball.setFillColor(sf::Color(0x808080FF));

    sf::RenderWindow window(sf::VideoMode(screenSize.x, screenSize.y), "Pong");
    sf::Clock clock;

    while(window.isOpen()) {

        sf::Event event;
        while(window.pollEvent(event)) {
            switch(event.type) {

                case sf::Event::Closed:
                    window.close();
                    break;
                
                case sf::Event::KeyPressed:
                    switch(event.key.code) {
                        case sf::Keyboard::Escape:
                            window.close();
                            break;
                    }
                    break;
            }
        }

        float dt = clock.restart().asSeconds();

        if(sf::Keyboard::isKeyPressed(sf::Keyboard::S))     posPlayer1.y += velPlayer * dt;
        if(sf::Keyboard::isKeyPressed(sf::Keyboard::W))     posPlayer1.y -= velPlayer * dt;
        if(sf::Keyboard::isKeyPressed(sf::Keyboard::Down))  posPlayer2.y += velPlayer * dt;
        if(sf::Keyboard::isKeyPressed(sf::Keyboard::Up))    posPlayer2.y -= velPlayer * dt;
        
        if(posPlayer1.y < 0) posPlayer1.y = 0;
        else if(posPlayer1.y > limitsPlayer.y) posPlayer1.y = limitsPlayer.y;
        if(posPlayer2.y < 0) posPlayer2.y = 0;
        else if(posPlayer2.y > limitsPlayer.y) posPlayer2.y = limitsPlayer.y;

        posBall += dirBall * dt;
        if(posBall.x < 0) {
            posBall.x = 0;
            dirBall.x = velBall;
            score2++;
        }
        else if(posBall.x > limitsBall.x) {
            posBall.x = limitsBall.x;
            dirBall.x = -velBall;
            score1++;
        }
        if(posBall.y < 0) {
            posBall.y = 0;
            dirBall.y = velBall;
        }
        else if(posBall.y > limitsBall.y) {
            posBall.y = limitsBall.y;
            dirBall.y = -velBall;
        }
        
        scoreText.setString(std::to_string(score1) + "  /  " + std::to_string(score2));
        posScorePanel.x = (screenSize.x - scoreText.getLocalBounds().width) / 2;

        if(sf::FloatRect(posPlayer1, sizePlayer).intersects(
            sf::FloatRect(posBall, sizeBall))) {
            posBall.x = posPlayer1.x + sizePlayer.x;
            dirBall.x = velBall;
        }
        if(sf::FloatRect(posPlayer2, sizePlayer).intersects(
            sf::FloatRect(posBall, sizeBall))) {
            posBall.x = posPlayer2.x - sizeBall.x;
            dirBall.x = -velBall;
        }

        player1.setPosition(posPlayer1);
        player2.setPosition(posPlayer2);
        scoreText.setPosition(posScorePanel);
        ball.setPosition(posBall);

        window.clear();
        window.draw(player1);
        window.draw(player2);
        window.draw(ball);
        window.draw(scoreText);
        window.display();
    }
    
    return 0;
}

