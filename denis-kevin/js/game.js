var gameProperties = {
    screenWidth: 640,
    screenHeight: 480,
};

var states = {
    game: "game",
};

// These states are represented by their index in the tank state array.
var tankStates = {
    moving: 0,
    turretRotating: 1,
    firing: 2,
    reloading: 3,
};
// Assume that tank only has one state.
var currentTankState = tankStates.moving;
var timeUntilSwitchAllowed = 0;
var switchDelay = 500;

var tank, turret, bullets;
var nextFire = 0, currentSpeed = 0;
var cursors, fireKey;
var fireRate = 100;
var bulletScale = 0.25;

var gameState = function(game) {
};

gameState.prototype = {
    preload: function() {
        game.load.image('bullet', 'assets/bullet.png');
        game.load.image('turret', 'assets/turret.png');
        game.load.image('tank','assets/tank.png');
    },

    create: function() {
        game.stage.backgroundColor = "#dddddd";

        game.world.setBounds(-gameProperties.screenWidth / 2,
            -gameProperties.screenHeight / 2,
            gameProperties.screenWidth,
            gameProperties.screenHeight);

        // Add tank
        tank = game.add.sprite(0, 0, 'tank');
        tank.anchor.setTo(0.5, 0.5);
        // moving, turretRotating, firing, reloading (initially can move only)
        tank.state = [true, false, false, false];

        game.physics.enable(tank, Phaser.Physics.ARCADE);
        tank.body.drag.set(0.2);
        tank.body.maxVelocity.setTo(400, 400);
        tank.body.collideWorldBounds = true;

        turret = game.add.sprite(0, 0, 'turret');
        turret.scale.setTo(0.75, 0.75);
        turret.anchor.setTo(0.3, 0.8);
        // game.physics.enable(turret, Phaser.Physics.ARCADE);
        // turret.body.


        bullets = game.add.group();
        bullets.enableBody = true;
        bullets.scale.setTo(bulletScale, bulletScale);
        bullets.physicsBodyType = Phaser.Physics.ARCADE;
        bullets.createMultiple(100, 'bullet', 0, false);
        bullets.setAll('anchor.x', 0.5);
        bullets.setAll('anchor.y', 0.5);
        bullets.setAll('outOfBoundsKill', true);
        bullets.setAll('checkWorldBounds', true);

        cursors = game.input.keyboard.createCursorKeys();
        fireKey = game.input.keyboard.addKey(Phaser.KeyCode.SPACEBAR);
        turretRotationKeys = game.input.keyboard.addKeys({"rotateLeft": Phaser.KeyCode.Q, "rotateRight": Phaser.KeyCode.E});
        stateSwitchingKeys = game.input.keyboard.addKeys({"moving": Phaser.KeyCode.ONE, "turretRotating": Phaser.KeyCode.TWO,
                                                            "firing": Phaser.KeyCode.THREE, "reloading": Phaser.KeyCode.FOUR});
    },

    update: function() {
        // Switching tank states
        timeUntilSwitchAllowed = Math.max(0, timeUntilSwitchAllowed - game.time.elapsed);
        if (timeUntilSwitchAllowed == 0) {
            if (stateSwitchingKeys.moving.isDown && !tank.state[tankStates.moving]) {
                tank.state[currentTankState] = false;
                tank.state[tankStates.moving] = true;
                currentTankState = tankStates.moving;
                timeUntilSwitchAllowed = switchDelay;
            } else if (stateSwitchingKeys.turretRotating.isDown && !tank.state[tankStates.turretRotating]) {
                tank.state[currentTankState] = false;
                tank.state[tankStates.turretRotating] = true;
                currentTankState = tankStates.turretRotating;
                timeUntilSwitchAllowed = switchDelay;
            } else if (stateSwitchingKeys.firing.isDown && !tank.state[tankStates.firing]) {
                tank.state[currentTankState] = false;
                tank.state[tankStates.firing] = true;
                currentTankState = tankStates.firing;
                timeUntilSwitchAllowed = switchDelay;
            } else if (stateSwitchingKeys.reloading.isDown && !tank.state[tankStates.reloading]) {
                tank.state[currentTankState] = false;
                tank.state[tankStates.reloading] = true;
                currentTankState = tankStates.reloading;
                timeUntilSwitchAllowed = switchDelay;
            }
        }

        // Tank movement
        if (tank.state[tankStates.moving]) {
            if (cursors.left.isDown) {
                tank.angle -= 4;
            }
            else if (cursors.right.isDown) {
                tank.angle += 4;
            } else if (cursors.up.isDown) {
                currentSpeed = 300;
            } else {
                if (currentSpeed > 0) {
                    currentSpeed -= 4;
                }
            }
        }

        if (currentSpeed > 0) {
            game.physics.arcade.velocityFromRotation(tank.rotation - Math.PI / 2, currentSpeed, tank.body.velocity);
        }

        // Attach turret to tank every iteration
        turret.x = tank.x;
        turret.y = tank.y;

        // Turning the turret
        if (tank.state[tankStates.turretRotating]) {
            if (turretRotationKeys.rotateLeft.isDown) {
                turret.angle -= 4;
            } else if (turretRotationKeys.rotateRight.isDown) {
                turret.angle += 4;
            }
        }

        // Fire bullets
        if (tank.state[tankStates.firing]) {
            if (fireKey.isDown)
            {
                fire();
            }
        }

    }


};

var fire = function() {

    if (game.time.now > nextFire && bullets.countDead() > 0) {
        nextFire = game.time.now + fireRate;

        var bullet = bullets.getFirstExists(false);
        var distanceOffset = 10;
        var offsetX = Math.cos(turret.rotation - Math.PI / 2) * turret.height + distanceOffset;
        var offsetY = Math.sin(turret.rotation - Math.PI / 2) * turret.height + distanceOffset;
        bullet.reset(1 / bulletScale *(turret.x + offsetX), 1 / bulletScale * (turret.y + offsetY));
        game.physics.arcade.velocityFromRotation(turret.rotation - Math.PI / 2, 200, bullet.body.velocity);
    }
}

var game = new Phaser.Game(gameProperties.screenWidth, gameProperties.screenHeight, Phaser.AUTO, 'gameDiv');
game.state.add(states.game, gameState);
game.state.start(states.game);
