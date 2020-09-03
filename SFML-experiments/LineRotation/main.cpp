/**
 * Compiling instructions
 *  - Create any directory, say "build" and cd into it.
 *  - run "cmake .."
 *  - run "make"
 *  - run "cd .."
 *  - run "build/LineRotation"
 */

#include <SFML/Graphics.hpp>

#define WINDOW_WIDTH  500
#define WINDOW_HEIGHT 500

#define LINE_SIZE      150.f
#define LINE_THICKNESS 2.f

int main() {
    sf::RenderWindow window(sf::VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT), "Line Rotation");

    sf::RectangleShape line(sf::Vector2f(LINE_SIZE, LINE_THICKNESS));
    line.move(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2);
    line.setOrigin(LINE_SIZE / 2, LINE_THICKNESS / 2);

    sf::Clock clock;

    sf::Font arialFont;
    if (!arialFont.loadFromFile("../assets/arial.ttf"))
        return 1;

    sf::Text fpsText;
    fpsText.setFont(arialFont);
    fpsText.setCharacterSize(16);
    fpsText.move(10, 10);
    fpsText.setFillColor(sf::Color::White);
    fpsText.setString("FPS: TBD");

    int fpsCounter = 0;
    float timeAccum = 0;

    while (window.isOpen()) {
        sf::Event event;
        while (window.pollEvent(event))
            if (event.type == sf::Event::Closed)
                window.close();
        
        float delta = clock.restart().asSeconds();

        line.rotate(90.f * delta);

        timeAccum += delta;
        if (timeAccum >= 1) {
            fpsText.setString("FPS: " + std::to_string(fpsCounter));
            
            fpsCounter = 0;
            timeAccum = 0;
        }

        fpsCounter++;

        window.clear();
        
        window.draw(line);
        window.draw(fpsText);

        window.display();
    }

    return 0;
}