/**
 * Compiling instructions
 *  - Create any directory, say "build" and cd into it.
 *  - run "cmake .."
 *  - run "make"
 *  - run "cd .."
 *  - run "build/hello_world"
 */


#include <SFML/Graphics.hpp>
#include <iostream>
#include <random>
#include <vector>

#define WINDOW_WIDTH    800
#define WINDOW_HEIGHT   600


int main() {

    ////////// LOAD ASSETS //////////
    sf::Texture dvdTexture;
    if(!dvdTexture.loadFromFile("assets/dvd.png")) {
        std::cout << "Unable to load DVD texture file :c" << std::endl;
        return 1;
    }

    sf::Sprite dvdSprite;
    dvdSprite.setTexture(dvdTexture);
    dvdSprite.setScale(0.1, 0.1);
    dvdSprite.setPosition(20, 20);

    sf::RenderWindow window(sf::VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT), "Hello wordl SFML!");
    sf::Clock clock;

    sf::Vector2f dvdSpeed(1, 1);

    while(window.isOpen()) {

        sf::Event event;
        while(window.pollEvent(event)) {
            switch (event.type)
            {
                case sf::Event::Closed:
                    window.close();
                    break;
                
                case sf::Event::KeyPressed:
                    switch (event.key.code)
                    {
                        case sf::Keyboard::Escape:
                            window.close();
                            break;
                    }
                    break;
            }
        }

        sf::Time time = clock.restart();

        dvdSprite.move(time.asSeconds() * 100 * dvdSpeed);
        sf::FloatRect dvdBounds = dvdSprite.getGlobalBounds();
        bool collided = false;
        if(dvdBounds.left < 0 || dvdBounds.left + dvdBounds.width > WINDOW_WIDTH) {
            collided = true;
            dvdSpeed.x *= -1;
        }
        if(dvdBounds.top < 0 || dvdBounds.top + dvdBounds.height > WINDOW_HEIGHT) {
            collided = true;
            dvdSpeed.y *= -1;
        }
        if(collided) {
            dvdSprite.move(time.asSeconds() * 100 * dvdSpeed);
            sf::Color current = dvdSprite.getColor();
            sf::Color newColor;
            while(newColor == sf::Color::Black || newColor == current)
                newColor = sf::Color(255 * (rand() & 1), 255 * (rand() & 1), 255 * (rand() & 1));
            dvdSprite.setColor(newColor);
        }

        window.clear();
        window.draw(dvdSprite);
        window.display();
    }

    return 0;
}
