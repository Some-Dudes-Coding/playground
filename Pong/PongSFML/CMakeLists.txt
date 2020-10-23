cmake_minimum_required(VERSION 3.10)
project(pong)
add_executable(pong main.cpp)
target_link_libraries(pong sfml-graphics sfml-window sfml-system)