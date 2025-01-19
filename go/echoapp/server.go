package main

import (
	"context"
	"fmt"
	"io"
	"log"
	"net/http"
	"os"
	"os/signal"
	"time"

	"github.com/labstack/echo/v4"
	"github.com/labstack/echo/v4/middleware"
)

const (
	myBirthday = "1976-12-25"
	dateFormat = "2006-01-02"
)

type User struct {
	Name  string `json:"name" xml:"name" form:"name" query:"name"`
	Email string `json:"email" xml:"email" form:"email" query:"email"`
}

// getUser handles GET requests to /users/:id.
func getUser(c echo.Context) error {
	id := c.Param("id")
	return c.String(http.StatusOK, id)
}

// show handles GET requests to /show
func show(c echo.Context) error {
	team := c.QueryParam("team")
	member := c.QueryParam("member")
	return c.String(http.StatusOK, "team:"+team+", member:"+member)
}

// save handles POST requests to /save.
func save(c echo.Context) error {
	name := c.FormValue("name")
	avatar, err := c.FormFile("avatar")
	if err != nil {
		return c.String(http.StatusBadRequest, "Failed to get avatar")
	}

	src, err := avatar.Open()
	if err != nil {
		return c.String(http.StatusBadRequest, "Failed to open avatar")
	}
	defer src.Close()

	dst, err := os.OpenFile(avatar.Filename, os.O_RDWR|os.O_CREATE|os.O_TRUNC, 0644)
	if err != nil {
		return c.String(http.StatusBadRequest, "Failed to create file")
	}
	defer dst.Close()

	bytesCopied, err := io.Copy(dst, src)
	if err != nil {
		return c.String(http.StatusBadRequest, "Failed to save avatar")
	}
	if bytesCopied != avatar.Size {
		return c.String(http.StatusInternalServerError, "Incomplete file copy")
	}

	return c.HTML(http.StatusOK, "<b>Thank you! "+name+"</b>")
}

// saveUser handles POST requests to /users.
func saveUser(c echo.Context) error {
	u := new(User)
	if err := c.Bind(u); err != nil {
		return c.String(http.StatusBadRequest, "Failed to bind user")
	}
	return c.JSON(http.StatusCreated, u)
}

// getDaysSinceBirthday handles GET requests to the root path.
func getDaysSinceBirthday(c echo.Context) error {
	dob, err := time.Parse("2006-01-02", myBirthday)
	if err != nil {
		return c.String(http.StatusBadRequest, "Invalid date format")
	}

	diff := time.Since(dob)
	days := int(diff.Hours() / 24)

	return c.String(http.StatusOK, fmt.Sprintf("Days since my birthday: %d", days))
}

// getUsers handles GET requests to /users.
func getUsers(c echo.Context) error {
	return c.String(http.StatusOK, "User list")
}

// CustomMW is tracking Custom-Header values.
func CustomMW(next echo.HandlerFunc) echo.HandlerFunc {
	return func(c echo.Context) error {
		fmt.Println("Custom header:", c.Request().Header.Get("Custom-Header"))
		return next(c)
	}
}

// trackMiddleware is tracking requests to /users.
func trackMiddleware(next echo.HandlerFunc) echo.HandlerFunc {
	return func(c echo.Context) error {
		fmt.Println("request to /users")
		return next(c)
	}
}

func main() {
	e := echo.New()

	// Root level middleware
	e.Use(middleware.Logger())
	e.Use(middleware.Recover())
	e.Use(CustomMW)

	e.GET("/", getDaysSinceBirthday)
	e.GET("/users", getUsers, trackMiddleware)
	e.POST("/users", saveUser)
	e.GET("/users/:id", getUser)
	e.GET("/show", show)
	e.POST("/save", save)
	e.Static("/static", "static")

	// Group level middleware
	g := e.Group("/admin")
	g.Use(middleware.BasicAuth(func(username, password string, c echo.Context) (bool, error) {
		if username == "joe" && password == "secret" {
			return true, nil
		}
		return false, nil
	}))

	// Start server
	go func() {
		if err := e.Start(":1323"); err != nil && err != http.ErrServerClosed {
			log.Fatalf("shutting down the server: %v", err)
		}
	}()

	// Wait for interrupt signal to gracefully shutdown the server with a timeout of 10 seconds.
	quit := make(chan os.Signal, 1)
	signal.Notify(quit, os.Interrupt)
	<-quit
	log.Println("Shutting down server...")

	ctx, cancel := context.WithTimeout(context.Background(), 10*time.Second)
	defer cancel()
	if err := e.Shutdown(ctx); err != nil {
		log.Fatal("Server forced to shutdown:", err)
	}

	log.Println("Server exiting")
}
